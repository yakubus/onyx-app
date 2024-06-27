import { z } from "zod";
import { SubcategorySchema } from "@/lib/validation/subcategory";
import {
  NameSchema,
  RequiredString,
  ResultSchema,
} from "@/lib/validation/base";

export const CategorySchema = z.object({
  id: RequiredString,
  name: RequiredString,
  subcategories: z.array(SubcategorySchema),
  optimistic: z.boolean().optional(),
});

export const CategoryResultSchema = ResultSchema.extend({
  value: z.array(CategorySchema),
});

export const CreateCategorySchema = z.object({
  name: NameSchema,
});
export type CreateCategory = z.infer<typeof CreateCategorySchema>;

export type Category = z.infer<typeof CategorySchema>;
