import axios from "axios";
import { userKey } from "@/components/AuthProvider";
import { getStoredUser } from "@/lib/utils";

const BASE_URL = "/api";

export const publicApi = axios.create({
  baseURL: BASE_URL,
});

export const privateApi = axios.create({
  baseURL: BASE_URL,
});

privateApi.interceptors.request.use(
  function (config) {
    const user = getStoredUser(userKey);

    if (user?.accessToken) {
      config.headers.Authorization = `Bearer ${user.accessToken}`;
    }

    return config;
  },
  function (error) {
    return Promise.reject(error);
  },
);
