import { useEffect, useState } from "react";
import { appointmentsApi } from "../api/appointments.api";
import type { AppointmentTypeDto } from "../dtos/appointment-type.dto";
import type { AppointmentDto } from "../dtos/appointment.dto";
import { getNowForDateTimeLocal } from "../utils/date.util";

type Props = {
  onClose: () => void;
  onCreated?: (a: AppointmentDto) => void;
  initialAppointment?: AppointmentDto;
  onUpdated?: (a: AppointmentDto) => void;
};

export default function CreateAppointmentModal({
  onClose,
  onCreated,
  initialAppointment,
  onUpdated,
}: Props) {
  const isEdit = !!initialAppointment;

  const [types, setTypes] = useState<AppointmentTypeDto[]>([]);
  const [selectedTypeId, setSelectedTypeId] = useState<number | "">(
    initialAppointment?.typeId ?? ""
  );
  const [scheduledTime, setScheduledTime] = useState(
    initialAppointment
      ? initialAppointment.scheduledTime.slice(0, 16)
      : ""
  );

  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadTypes() {
      const res = await appointmentsApi.getTypes();
      setTypes(res.data);
      setLoading(false);
    }
    loadTypes();
  }, []);

  async function handleSubmit() {
    if (!selectedTypeId || !scheduledTime) return;

    try {
      setSubmitting(true);

      const payload = {
        typeId: selectedTypeId,
        scheduledTime: new Date(scheduledTime).toISOString(),
      };

      if (isEdit && initialAppointment && onUpdated) {
         await appointmentsApi.update(
          initialAppointment.appointmentId,
          payload
        );
          onUpdated({
            ...initialAppointment,
            scheduledTime: payload.scheduledTime,
          });
        
      }

      if (!isEdit && onCreated) {
        const res = await appointmentsApi.create(payload);
        onCreated(res.data);
      }

      onClose();
    } catch {
      setError("Operation failed");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center">
      <div className="bg-white rounded p-6 w-[360px]">
        <h2 className="font-semibold mb-4">
          {isEdit ? "Update Appointment" : "Create Appointment"}
        </h2>

        {loading ? (
          <div>Loading…</div>
        ) : (
          <>
            <select
              value={selectedTypeId}
              onChange={(e) => setSelectedTypeId(Number(e.target.value))}
              className="w-full border p-2 mb-3"
            >
              <option value="">Select dog type</option>
              {types.map((t) => (
                <option key={t.typeId} value={t.typeId}>
                  {t.typeName}
                </option>
              ))}
            </select>

            <input
              type="datetime-local"
              min={getNowForDateTimeLocal()}
              value={scheduledTime}
              onChange={(e) => setScheduledTime(e.target.value)}
              className="w-full border p-2 mb-3"
            />

            {error && <div className="text-red-600 mb-2">{error}</div>}

            <div className="flex justify-end gap-3">
              <button onClick={onClose}>Cancel</button>
              <button
                onClick={handleSubmit}
                disabled={submitting}
                className="bg-indigo-600 text-white px-4 py-2 rounded"
              >
                {isEdit ? "Update" : "Create"}
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}
