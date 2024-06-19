import { createFileRoute, redirect } from "@tanstack/react-router";

import { LayoutSearchParamsSchema } from "@/lib/validation/searchParams";

export const Route = createFileRoute("/_dashboard-layout")({
  beforeLoad: ({ context: { auth }, location }) => {
    if (!auth.isAuthenticated) {
      throw redirect({
        to: "/",
        search: {
          redirect: location.href,
        },
      });
    }
  },
  validateSearch: LayoutSearchParamsSchema,
});
