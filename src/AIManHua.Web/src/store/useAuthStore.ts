import { create } from "zustand";

export interface AuthUser {
  userId: number;
  username: string;
  email: string;
}

interface AuthState {
  user: AuthUser | null;
  token: string | null;
  expiresAt: string | null;
  setAuth: (user: AuthUser, token: string, expiresAt: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
}

const saved = (() => {
  try {
    const u = localStorage.getItem("aimanhua_user");
    const t = localStorage.getItem("aimanhua_token");
    const e = localStorage.getItem("aimanhua_expires");
    if (u && t) return { user: JSON.parse(u) as AuthUser, token: t, expiresAt: e };
  } catch { /* ignore */ }
  return { user: null, token: null, expiresAt: null };
})();

export const useAuthStore = create<AuthState>((set, get) => ({
  user: saved.user,
  token: saved.token,
  expiresAt: saved.expiresAt,
  setAuth: (user, token, expiresAt) => {
    localStorage.setItem("aimanhua_user", JSON.stringify(user));
    localStorage.setItem("aimanhua_token", token);
    if (expiresAt) localStorage.setItem("aimanhua_expires", expiresAt);
    set({ user, token, expiresAt });
  },
  logout: () => {
    localStorage.removeItem("aimanhua_user");
    localStorage.removeItem("aimanhua_token");
    localStorage.removeItem("aimanhua_expires");
    set({ user: null, token: null, expiresAt: null });
  },
  isLoggedIn: () => {
    const { user, token } = get();
    return !!(user && token);
  },
}));
