import { z } from "zod";

export const CounterpartySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  type: z.enum(["Payee, Payer"]),
});
