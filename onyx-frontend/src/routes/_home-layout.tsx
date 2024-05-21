import { Outlet, createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_home-layout")({
  component: () => (
    <div>
      Hello /_home-layout!
      <Outlet />
    </div>
  ),
});
