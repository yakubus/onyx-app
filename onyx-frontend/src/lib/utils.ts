import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

import type { UserWithToken } from "@/lib/validation/user";

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

export const addSpacesToAmount = (amount: string) => {
  const parts = amount.split(".");
  // Apply spaces to the integer part
  parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");

  return parts.join(".");
};

export const formatDecimals = (amount: string) => {
  const parts = amount.split(".");

  if (parts[1]) {
    if (parts[1].length === 1) {
      parts[1] += "0";
    } else {
      parts[1] = parts[1].substring(0, 2);
    }
  } else {
    parts[1] = "00";
  }
  return parts.join(".");
};

export const formatAmount = (amount: string) => {
  const parts = amount.split(".");

  parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");

  if (parts[1]) {
    if (parts[1].length === 1) {
      parts[1] += "0";
    } else {
      parts[1] = parts[1].substring(0, 2);
    }
  } else {
    parts[1] = "00";
  }
  return parts.join(".");
};

export const removeSpacesFromAmount = (amount: string) => {
  return amount.replace(/ /g, "");
};
