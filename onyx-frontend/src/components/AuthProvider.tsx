import {
  ReactNode,
  createContext,
  useCallback,
  useEffect,
  useState,
} from "react";

import { UserWithToken } from "@/lib/validation/user";
import { getStoredUser, setStoredUser } from "@/lib/utils";

export interface AuthContextType {
  isAuthenticated: boolean;
  login: (user: UserWithToken) => Promise<void>;
  logout: () => Promise<void>;
  user: UserWithToken | null;
}

export const userKey = "tanstack.auth.user";

export const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserWithToken | null>(
    getStoredUser(userKey),
  );
  const isAuthenticated = !!user;

  const logout = useCallback(async () => {
    setStoredUser(null, userKey);
    setUser(null);
  }, []);

  const login = useCallback(async (user: UserWithToken) => {
    setStoredUser(user, userKey);
    setUser(user);
  }, []);

  useEffect(() => {
    setUser(getStoredUser(userKey));
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
