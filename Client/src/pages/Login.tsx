import { useState } from "react";
import { loginApi } from "../api/auth.api";
import RegisterModal from "../components/RegisterModal";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [showRegister, setShowRegister] = useState(false);

  const navigate = useNavigate();
  const { login } = useAuth();

  async function handleLogin() {
    try {
      setError("");
      const token = await loginApi(username, password);
      login(token.token);
      navigate("/appointments");
    } catch {
      setError("Invalid username or password");
    }
  }

return (
  <>
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md">

        {/* Logo + Title */}
        <div className="text-center mb-6">
          <div className="mx-auto h-12 w-12 rounded-2xl bg-indigo-600 text-white
                          flex items-center justify-center text-lg font-bold shadow-sm">
            P
          </div>

          <h1 className="mt-4 text-2xl font-semibold text-gray-900">
            PetQueue
          </h1>

          <p className="text-sm text-gray-500">
            Dog grooming appointment system
          </p>
        </div>

        {/* Card */}
        <div className="bg-white rounded-2xl shadow-lg border border-gray-200 p-8 space-y-5">

          <div className="text-center">
            <h2 className="text-xl font-semibold text-gray-900">
              Sign in
            </h2>
            <p className="text-sm text-gray-500">
              Enter your credentials
            </p>
          </div>

          {/* Inputs */}
          <div className="space-y-4">
            <input
              type="text"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="w-full rounded-xl border border-gray-300 px-4 py-3 text-sm
                         focus:outline-none focus:ring-2 focus:ring-indigo-500
                         focus:border-indigo-500 transition"
            />

            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full rounded-xl border border-gray-300 px-4 py-3 text-sm
                         focus:outline-none focus:ring-2 focus:ring-indigo-500
                         focus:border-indigo-500 transition"
            />
          </div>

          {/* Error */}
          {error && (
            <div className="text-sm text-red-600 text-center bg-red-50
                            border border-red-200 rounded-lg py-2">
              {error}
            </div>
          )}

          {/* Buttons */}
          <div className="space-y-3 pt-2">
            <button
              onClick={handleLogin}
              className="w-full rounded-xl bg-indigo-600 text-white py-3 text-sm font-medium
                         hover:bg-indigo-700 active:scale-[0.99] transition"
            >
              Login
            </button>

            <button
              onClick={() => setShowRegister(true)}
              className="w-full rounded-xl border border-indigo-600 text-indigo-600 py-3 text-sm
                         font-medium hover:bg-indigo-50 transition"
            >
              Create account
            </button>
          </div>
        </div>
      </div>
    </div>

    {showRegister && (
      <RegisterModal onClose={() => setShowRegister(false)} />
    )}
  </>
);

}
