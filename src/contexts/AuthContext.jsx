import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { login as apiLogin } from "../mocks/mockApi";

const AuthCtx = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    try {
      const raw = localStorage.getItem("cp_currentUser");
      if (raw) setUser(JSON.parse(raw));
    } catch {}
    setLoading(false);
  }, []);

  async function login(email, password, role) {
    const u = await apiLogin(email, password, role);
    setUser(u);
    localStorage.setItem("cp_currentUser", JSON.stringify(u));
    return u;
  }

  function logout() {
    setUser(null);
    localStorage.removeItem("cp_currentUser");
  }

  const value = useMemo(
    () => ({ user, loading, login, logout }),
    [user, loading]
  );
  return <AuthCtx.Provider value={value}>{children}</AuthCtx.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthCtx);
  if (!ctx) throw new Error("useAuth must be used within <AuthProvider>");
  return ctx;
}
