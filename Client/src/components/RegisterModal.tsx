import { useState } from "react";
import { registerApi } from "../api/auth.api";
import { useAuth } from "../auth/useAuth";

type Props = {
  onClose: () => void;
};

export default function RegisterModal({ onClose }: Props) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [firstName, setFirstName] = useState("");
  const [error, setError] = useState("");

  const { login } = useAuth();

  async function handleRegister() {
    try {
      setError("");
      const res = await registerApi(username, password, firstName);
      login(res.token);
      onClose();
    } catch {
      setError("Username already exists");
    }
  }

  const disabled = !username || !password || !firstName;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4">
      <div className="w-full max-w-sm bg-white rounded-2xl shadow-lg border border-gray-200 p-6">
        {/* Header */}
        <div className="text-center mb-4">
          <h2 className="text-2xl font-semibold text-gray-900">
            Create account
          </h2>
          <p className="text-sm text-gray-500">
            Quick registration
          </p>
        </div>

        {/* Form */}
        <div className="space-y-3">
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                       focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                       focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />

          <input
            type="text"
            placeholder="First name"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm
                       focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        {error && (
          <div className="mt-2 text-sm text-red-600 text-center">
            {error}
          </div>
        )}

        {/* Actions */}
        <div className="mt-6 flex justify-end gap-3">
          <button
            onClick={onClose}
            className="rounded-lg border border-gray-300 px-4 py-2 text-sm
                       text-gray-700 hover:bg-gray-100 transition"
          >
            Cancel
          </button>

          <button
            onClick={handleRegister}
            disabled={disabled}
            className={`rounded-lg px-4 py-2 text-sm font-medium text-white transition
              ${
                disabled
                  ? "bg-indigo-400 cursor-not-allowed"
                  : "bg-indigo-600 hover:bg-indigo-700"
              }`}
          >
            Create account
          </button>
        </div>
      </div>
    </div>
  );
}
