import { z } from "zod";

export const SingleBudgetPageParamsSchema = z.object({
  selectedBudget: z.string(),
  month: z
    .string()
    .regex(/^\d{1,2}$/)
    .refine(
      (val) => {
        const monthNumber = parseInt(val, 10);
        return monthNumber >= 1 && monthNumber <= 12;
      },
      {
        message: "Month must be a number between 1 and 12",
      },
    ),
  year: z.string().refine((val) => val === "2024", {
    message: "Year must be 2024",
  }),
});

export const LayoutSearchParamsSchema = z.object({
  selectedBudget: z.string().optional(),
});
