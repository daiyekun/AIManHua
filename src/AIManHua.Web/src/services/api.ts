import axios from "axios";

const apiClient = axios.create({
  baseURL: "/api",
  timeout: 30000,
  headers: { "Content-Type": "application/json" },
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("aimanhua_token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    let msg = "请求失败，请稍后重试";
    if (error.response?.data) {
      const data = error.response.data;
      msg = data.message || data.detail || data.title || msg;
      if (data.errors) {
        const first = Object.values(data.errors).flat()[0];
        if (first) msg = first as string;
      }
    } else if (error.message) {
      msg = error.message;
    }
    return Promise.reject(new Error(msg));
  }
);

export default apiClient;

// ── Auth API ──

export interface AuthPayload {
  email: string;
  username?: string;
  password: string;
}

export interface AuthResult {
  userId: number;
  username: string;
  email: string;
  accessToken: string;
  expiresAt: string;
}

export async function registerApi(payload: AuthPayload): Promise<AuthResult> {
  const { data } = await apiClient.post<AuthResult>("/auth/register", payload);
  return data;
}

export async function loginApi(payload: AuthPayload): Promise<AuthResult> {
  const { data } = await apiClient.post<AuthResult>("/auth/login", payload);
  return data;
}

export async function getMeApi(): Promise<{ id: number; username: string; email: string }> {
  const { data } = await apiClient.get("/auth/me");
  return data;
}
