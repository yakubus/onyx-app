import { Outlet, createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout")({
  component: () => (
    <div>
      Hello /_dashboard-layout!
      <Outlet />
    </div>
  ),
});
