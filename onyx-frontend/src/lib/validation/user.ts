import { z } from "zod";
import { ResultSchema } from "./base";

export const UserSchema = z.object({
  id: z.string(),
  username: z.string(),
  email: z.string().email(),
  currency: z.string(),
  budgetIds: z.array(z.string()),
});

export const UserResultSchema = ResultSchema.extend({
  value: UserSchema,
});

export type User = z.infer<typeof UserSchema>;
