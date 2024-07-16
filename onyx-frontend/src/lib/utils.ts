import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

import type { UserWithToken } from "@/lib/validation/user";
import { USER_LOCALE } from "./constants/locale";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const getErrorMessage = (error: unknown): string => {
  let message: string;

  if (error instanceof Error) {
    message = error.message;
  } else if (error && typeof error === "object" && "message" in error) {
    message = String(error.message);
  } else if (typeof error === "string") {
    message = error;
  } else {
    message = "Something went wrong";
  }

  return message;
};

export const capitalize = (str: string) =>
  str.charAt(0).toUpperCase() + str.slice(1);

export function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

export function getStoredUser(key: string): UserWithToken | null {
  return localStorage.getItem(key)
    ? JSON.parse(localStorage.getItem(key)!)
    : null;
}

export function setStoredUser(user: UserWithToken | null, key: string) {
  if (user) {
    localStorage.setItem(key, JSON.stringify(user));
  } else {
    localStorage.removeItem(key);
  }
}

export const getFormattedCurrency = (amount: number, currency: string) => {
  return new Intl.NumberFormat(USER_LOCALE, {
    style: "currency",
    currency,
  }).format(amount);
};

export const formatToDotDecimal = (str: string): string => {
  const index = str.indexOf(",");

  if (index !== -1) {
    return str.substring(0, index) + "." + str.substring(index + 1);
  }

  return str;
};

export const isCurrentDate = (month: string, year: string) =>
  Number(month) === new Date().getMonth() + 1 &&
  Number(year) === new Date().getFullYear();
