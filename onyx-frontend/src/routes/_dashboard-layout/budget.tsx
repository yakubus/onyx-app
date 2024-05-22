import { CategoryResultSchema } from "@/lib/validation/api";
import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/budget")({
  component: Budget,
});

function Budget() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["categories"],
    queryFn: async () => {
      const res = await fetch("/api/categories");
      if (!res.ok) {
        throw new Error("API error");
      }
      const result = await res.json();
      const validatedResult = CategoryResultSchema.safeParse(result);
      if (validatedResult.error) {
        console.error(validatedResult.error.errors);
        throw new Error("Invalid data type.");
      }

      const { isFailure, error, value } = validatedResult.data;

      if (isFailure) {
        throw new Error(error.message);
      }

      return value;
    },
  });

  if (isLoading) {
    return <div>...loading</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return (
    <div>
      <pre>{JSON.stringify(data)}</pre>
    </div>
  );
}
