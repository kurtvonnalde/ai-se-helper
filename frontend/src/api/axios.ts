import axios from "axios";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? "/api";

export const api = axios.create({
  baseURL: apiBaseUrl,
  headers: {
    "Content-Type": "application/json",
  },
});

type ProblemDetailsResponse = {
  title?: string;
  status?: number;
  detail?: string;
  traceId?: string;
  code?: string;
};

export type ApiError = {
  message: string;
  status: number;
  code: string;
  traceId?: string;
};

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (!axios.isAxiosError(error)) {
      const fallback: ApiError = {
        message: "Unexpected error occurred.",
        status: 500,
        code: "unknown_error",
      };
      return Promise.reject(fallback);
    }

    const data = (error.response?.data ?? {}) as ProblemDetailsResponse;

    const normalized: ApiError = {
      message: data.detail || error.message || "Request failed.",
      status: error.response?.status ?? 500,
      code: data.code || data.title || "api_error",
      traceId: data.traceId,
    };

    return Promise.reject(normalized);
  }
);