export const MONTHS = [
  "January",
  "February",
  "March",
  "April",
  "May",
  "June",
  "July",
  "August",
  "September",
  "October",
  "November",
  "December",
] as const;

export const MIN_YEAR = 2024;
export const DEFAULT_MONTH_NUMBER = new Date().getMonth() + 1;
export const DEFAULT_MONTH_STRING = (new Date().getMonth() + 1).toString();
export const DEFAULT_YEAR_STRING = new Date().getFullYear().toString();
export const DEFAULT_YEAR_NUMBER = new Date().getFullYear();
