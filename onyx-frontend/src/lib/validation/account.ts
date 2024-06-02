import { z } from "zod";
import { MoneySchema, ResultSchema } from "@/lib/validation/base";

export const AccountSchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  balance: MoneySchema,
  type: z.enum(["Checking", "Saving"]),
});

export const AccountResultSchema = ResultSchema.extend({
  value: z.array(AccountSchema),
});
export type Account = z.infer<typeof AccountSchema>;
