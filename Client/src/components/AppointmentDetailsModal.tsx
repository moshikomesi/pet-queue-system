import type { AppointmentDto } from "../dtos/appointment.dto";
import { formatDateTime } from "../utils/date.util";

type Props = {
  appointment: AppointmentDto;
  canModify: boolean;
  onClose: () => void;
  onEdit: () => void;
  onDelete: () => void;
};

export default function AppointmentDetailsModal({
  appointment,
  canModify,
  onClose,
  onEdit,
  onDelete,
}: Props) {
  return (
    <div className="fixed inset-0 z-50 bg-black/50 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-xl shadow-lg p-6">
        <h2 className="text-xl font-semibold mb-4">
          Appointment details
        </h2>

        <div className="space-y-3 text-sm">
          <Row label="Client" value={appointment.clientName} />
          <Row label="Dog size" value={appointment.dogSize} />
          <Row
            label="Scheduled"
            value= {formatDateTime(new Date(appointment.scheduledTime).toLocaleString())}
          />
          <Row
            label="Created"
            value={formatDateTime(new Date(appointment.createdAt).toLocaleString())}
          />
          <Row label="Price" value={`₪${appointment.finalPrice}`} />
        </div>

        <div className="mt-6 flex justify-between items-center">
          {canModify && (
            <button
              onClick={onDelete}
              className="text-red-600 text-sm hover:underline"
            >
              Delete
            </button>
          )}

          <div className="flex gap-3">
            <button
              onClick={onClose}
              className="px-4 py-2 text-sm border rounded-lg"
            >
              Close
            </button>

            {canModify && (
              <button
                onClick={onEdit}
                className="px-4 py-2 text-sm bg-indigo-600 text-white rounded-lg"
              >
                Edit
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

function Row({ label, value }: { label: string; value: string }) {
  return (
    <div className="flex justify-between">
      <span className="text-gray-500">{label}</span>
      <span className="font-medium text-gray-900">{value}</span>
    </div>
  );
}
