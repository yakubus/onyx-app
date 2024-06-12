import { z } from "zod";

export const ErrorSchema = z.object({
  code: z.string(),
  message: z.string(),
});

export const MoneySchema = z.object({
  currency: z.string().min(1),
  amount: z.number(),
});

export type Money = z.infer<typeof MoneySchema>;

export const MonthDateSchema = z.object({
  month: z.number(),
  year: z.number(),
});

export const ResultSchema = z.object({
  isSuccess: z.boolean(),
  isFailure: z.boolean(),
  error: ErrorSchema,
});

export const TargetSchema = z.object({
  upToMonth: MonthDateSchema,
  startedAt: MonthDateSchema,
  targetAmount: MoneySchema,
  collectedAmount: MoneySchema,
  amountAssignedEveryMonth: MoneySchema,
});

export const AssignmentSchema = z.object({
  month: MonthDateSchema,
  assignedAmount: MoneySchema,
  actualAmount: MoneySchema,
});
