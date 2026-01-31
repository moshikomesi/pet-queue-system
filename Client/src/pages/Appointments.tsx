import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";
import CreateAppointmentModal from "../components/CreateAppointmentModal";
import type { AppointmentDto } from "../dtos/appointment.dto";
import { appointmentsApi } from "../api/appointments.api";
import { getUserIdFromToken } from "../auth/jwt";
import AppointmentDetailsModal from "../components/AppointmentDetailsModal";
import { formatDateTime } from "../utils/date.util";
import TopBar from '../components/TopBar';


export default function Appointments() {
  const [appointments, setAppointments] = useState<AppointmentDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const UserId = getUserIdFromToken();
  const [editingAppointment, setEditingAppointment] = useState<AppointmentDto | null>(null);
  const [selectedAppointment, setSelectedAppointment] =
  useState<AppointmentDto | null>(null);  

  // filters
  const [nameFilter, setNameFilter] = useState("");
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  // modal
  const [showCreate, setShowCreate] = useState(false);

  // const navigate = useNavigate();
  // const { logout } = useAuth();


function resetFilters() {
  setNameFilter("");
  setFromDate("");
  setToDate("");
}


async function handleDelete(id: number) {
  const confirmed = window.confirm("Are you sure you want to delete this appointment?");
  if (!confirmed) return;

  try {
    const res = await appointmentsApi.delete(id);

    if (res.status === 200 || res.status === 204) {
      resetFilters();
      await loadAppointments();
    }
  } catch {
    alert("Failed to delete appointment");
  }
}


  function isToday(date: string) {
  const d = new Date(date);
  const now = new Date();

  return (
    d.getUTCFullYear() === now.getUTCFullYear() &&
    d.getUTCMonth() === now.getUTCMonth() &&
    d.getUTCDate() === now.getUTCDate()
  );
}


function canModify(a: AppointmentDto) {
  return ( 
   a.userId === Number(UserId)&&
    !isToday(a.scheduledTime)
  );
}
  async function loadAppointments() {
  try {
    setLoading(true);
    const res = await appointmentsApi.getAll();
    setAppointments(res.data);
  } catch {
    setError("Failed to load appointments");
  } finally {
    setLoading(false);
  }
}

  useEffect(() => {

    loadAppointments();
  }, []);

  const filteredAppointments = useMemo(() => {
    return appointments.filter((a) => {
      const matchName = a.clientName
        .toLowerCase()
        .includes(nameFilter.toLowerCase());

      const date = new Date(a.scheduledTime).getTime();
      const from = fromDate ? new Date(fromDate).getTime() : null;
      const to = toDate ? new Date(toDate).getTime() : null;

      const matchFrom = from ? date >= from : true;
      const matchTo = to ? date <= to : true;

      return matchName && matchFrom && matchTo;
    });
  }, [appointments, nameFilter, fromDate, toDate]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64 text-gray-500">
        Loading appointments…
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center h-64 text-red-500">
        {error}
      </div>
    );
  }

return (
  <div className="min-h-screen bg-gray-100">

    <TopBar onCreateClick={() => setShowCreate(true)} />

    {/* Page Content */}
    <div className="max-w-6xl mx-auto px-6 py-8">

      {/* Title */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold text-gray-900">
          Appointments
        </h2>
        <p className="text-sm text-gray-500">
          Overview of scheduled appointments • Showing {filteredAppointments.length}
        </p>
      </div>

      {/* Layout */}
      <div className="grid grid-cols-1 lg:grid-cols-[260px_1fr] gap-8">

        {/* Filters */}
        <aside className="bg-white rounded-xl shadow-sm border border-gray-200 p-5 h-fit">
          <h3 className="text-sm font-semibold text-gray-700 mb-5">
            Filters
          </h3>

          <div className="space-y-5">

            <div>
              <label className="block text-xs font-medium text-gray-500 mb-1">
                Client name
              </label>
              <input
                type="text"
                placeholder="Search client…"
                value={nameFilter}
                onChange={(e) => setNameFilter(e.target.value)}
                className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                           focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-gray-500 mb-1">
                From date
              </label>
              <input
                type="date"
                value={fromDate}
                onChange={(e) => setFromDate(e.target.value)}
                className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                           focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-gray-500 mb-1">
                To date
              </label>
              <input
                type="date"
                value={toDate}
                onChange={(e) => setToDate(e.target.value)}
                className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                           focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>

          </div>
        </aside>

        {/* Table */}
        <div className="flex justify-center">
          <section className="bg-white rounded-xl shadow-sm border border-gray-200
                              w-full max-w-2xl overflow-hidden">

            <table className="w-full text-sm table-fixed">

              <thead className="bg-gray-50 border-b border-gray-200">
                <tr>
                  <th className="px-5 py-3 text-left font-medium text-gray-500 w-1/2">
                    Client
                  </th>
                  <th className="px-5 py-3 text-left font-medium text-gray-500 w-1/2">
                    Scheduled
                  </th>
                </tr>
              </thead>

              <tbody className="divide-y divide-gray-100">
                {filteredAppointments.map((a) => (
                  <tr
                    key={a.appointmentId}
                    onClick={() => setSelectedAppointment(a)}
                    className="cursor-pointer hover:bg-gray-50 transition"
                  >
                    <td className="px-5 py-3 font-medium text-gray-900 truncate">
                      {a.clientName}
                    </td>

                    <td className="px-5 py-3 text-gray-700">
                      {formatDateTime(a.scheduledTime)}
                    </td>
                  </tr>
                ))}

                {filteredAppointments.length === 0 && (
                  <tr>
                    <td colSpan={2} className="text-center py-12 text-gray-400">
                      No appointments found
                    </td>
                  </tr>
                )}
              </tbody>

            </table>
          </section>
        </div>

      </div>
    </div>

    {/* Modals */}

    {showCreate && (
      <CreateAppointmentModal
        onClose={() => setShowCreate(false)}
        onCreated={async () => {
          setShowCreate(false);
          resetFilters();
          await loadAppointments();
        }}
      />
    )}

    {editingAppointment && (
      <CreateAppointmentModal
        initialAppointment={editingAppointment}
        onClose={() => setEditingAppointment(null)}
        onUpdated={async () => {
          setEditingAppointment(null);
          resetFilters();
          await loadAppointments();
        }}
      />
    )}

    {selectedAppointment && (
      <AppointmentDetailsModal
        appointment={selectedAppointment}
        canModify={canModify(selectedAppointment)}
        onClose={() => setSelectedAppointment(null)}
        onEdit={() => {
          setEditingAppointment(selectedAppointment);
          setSelectedAppointment(null);
        }}
        onDelete={async () => {
          await handleDelete(selectedAppointment.appointmentId);
          setSelectedAppointment(null);
        }}
      />
    )}

  </div>
);

}

