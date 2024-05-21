import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/budget")({
  component: Budget,
});

function Budget() {
  const { data } = useQuery({
    queryKey: ["accounts"],
    queryFn: async () => {
      const res = await fetch("/api/categories");
      if (!res.ok) {
        throw new Error("api error");
      }
      const accounts = res.json();
      return accounts;
    },
  });

  return (
    <div className="p-10">
      <div className="">
        <pre>{JSON.stringify(data)}</pre>
      </div>
    </div>
  );
}
