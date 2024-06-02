import { z } from "zod";
import { SubcategorySchema } from "@/lib/validation/subcategory";
import { ResultSchema } from "@/lib/validation/base";

export const CategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  subcategories: z.array(SubcategorySchema),
});

export const CategoryResultSchema = ResultSchema.extend({
  value: z.array(CategorySchema),
});

export const CreateCategorySchema = z.object({
  name: z
    .string()
    .min(1, "Please provide category name.")
    .regex(/^[a-zA-Z0-9\s.-]{1,50}$/, "Invalid category name."),
});
export type CreateCategory = z.infer<typeof CreateCategorySchema>;

export type Category = z.infer<typeof CategorySchema>;
