import { z } from "zod";

export const LayoutSearchParamsSchema = z.object({
  selectedBudget: z.string().optional(),
});
