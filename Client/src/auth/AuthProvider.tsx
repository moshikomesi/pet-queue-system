import { useState } from "react";
import type { ReactNode } from "react";
import { AuthContext } from "./AuthContext";
import type { AuthContextType, User } from "./auth.types";
import { TOKEN_KEY } from "./token";

type Props = { children: ReactNode };

export function AuthProvider({ children }: Props) {
  
  const [token, setToken] = useState<string | null>(() =>
    localStorage.getItem(TOKEN_KEY)
  );
  const [user, setUser] = useState<User | null>(null);

  const login: AuthContextType["login"] = (jwt) => {
      localStorage.setItem("token", jwt);
    setToken(jwt);
  };

  const logout: AuthContextType["logout"] = () => {
    localStorage.removeItem(TOKEN_KEY);
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ token, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
