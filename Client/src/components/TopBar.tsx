import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

type Props = {
  onCreateClick?: () => void;
};

export default function TopBar({ onCreateClick }: Props) {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <header className="bg-white border-b border-gray-200">
      <div className="max-w-7xl mx-auto px-6 h-16 flex items-center justify-between">
        {/* Left */}
        <div className="flex items-center gap-3">
          <div className="h-9 w-9 rounded-xl bg-indigo-600 text-white flex items-center justify-center font-semibold">
            P
          </div>

          <h1 className="text-lg font-semibold text-gray-900">
            PetQueue
          </h1>
        </div>

        {/* Right */}
        <div className="flex items-center gap-3">
          {onCreateClick && (
            <button
              onClick={onCreateClick}
              className="rounded-lg bg-indigo-600 text-white px-4 py-2 text-sm
                         font-medium hover:bg-indigo-700 transition"
            >
              Create
            </button>
          )}

          <button
            onClick={() => {
              logout();
              navigate("/login");
            }}
            className="rounded-lg border border-gray-300 px-4 py-2 text-sm
                       text-gray-700 hover:bg-gray-100 transition"
          >
            Logout
          </button>
        </div>
      </div>
    </header>
  );
}
