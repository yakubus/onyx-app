import {
  Link,
  Outlet,
  createFileRoute,
  redirect,
} from "@tanstack/react-router";

import Logo from "@/components/Logo";
import MobileNavigation from "@/components/dashboard/MobileNavigation";
import UserDropdown from "@/components/dashboard/UserDropdown";

import { useMediaQuery } from "@/lib/hooks/useMediaQuery";
import { LayoutSearchParamsSchema } from "@/lib/validation/searchParams";
import { cn } from "@/lib/utils";
import { BUDGET_LINKS, SIDEBAR_BOTTOM_LINKS } from "@/lib/constants/links";

export const Route = createFileRoute("/_dashboard-layout")({
  component: Layout,
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

function Layout() {
  const isDesktop = useMediaQuery("(min-width: 1024px)");
  const { selectedBudget } = Route.useSearch();

  if (!isDesktop) return <MobileLayout selectedBudget={selectedBudget} />;

  return (
    <div className="flex h-screen flex-col bg-background">
      <div className="mx-auto w-full max-w-screen-2xl flex-grow border-r shadow-xl shadow-primaryDark">
        <div className="grid h-full w-full grid-cols-5">
          <aside className="col-span-1 flex h-full flex-col bg-primaryDark py-10 text-primaryDark-foreground">
            <Link to="/budget">
              <Logo />
            </Link>
            <div className="flex flex-grow flex-col space-y-10 py-16 pl-10">
              <Link
                to={selectedBudget ? `/budget/${selectedBudget}` : "/budget"}
                search={(prev) => prev}
                mask={selectedBudget && { to: `/budget/${selectedBudget}` }}
                className="rounded-l-full py-4 pl-9 transition-all duration-300 hover:bg-background hover:text-foreground"
                activeProps={{
                  className: "bg-background text-foreground",
                }}
                preload="intent"
              >
                Budget
              </Link>
              {BUDGET_LINKS.map(({ label, href }) => (
                <Link
                  key={label}
                  to={href}
                  search={(prev) => prev}
                  mask={{ to: `/${href}` }}
                  className={cn(
                    "grid grid-rows-[0fr] rounded-l-full pl-9 transition-all duration-300 ease-in-out hover:bg-background hover:text-foreground",
                    selectedBudget && "grid-rows-[1fr] py-4",
                  )}
                  activeProps={{
                    className: "bg-background text-foreground",
                  }}
                  preload="intent"
                >
                  <span className="block overflow-hidden">{label}</span>
                </Link>
              ))}
            </div>
            <div className="flex flex-col space-y-4 py-16 pl-10">
              {SIDEBAR_BOTTOM_LINKS.map(({ label, href }) => (
                <Link
                  to={href}
                  className="rounded-l-full py-4 pl-9 transition-all duration-300 hover:bg-background hover:text-foreground"
                  activeProps={{
                    className: "bg-background text-foreground",
                  }}
                  preload="intent"
                >
                  {label}
                </Link>
              ))}
            </div>
          </aside>
          <div className="fixed left-0 top-0 w-full">
            <nav className="mx-auto max-w-screen-2xl px-28 py-10 text-end">
              <UserDropdown />
            </nav>
          </div>
          <main className="col-span-4 h-screen px-8 pb-4 pt-28">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}

const MobileLayout = ({
  selectedBudget,
}: {
  selectedBudget: string | undefined;
}) => {
  return (
    <>
      <nav className="fixed z-50 flex w-full items-center justify-between bg-primaryDark px-4 py-2 text-primaryDark-foreground md:px-8">
        <MobileNavigation selectedBudget={selectedBudget} />
        <Link to="/budget">
          <Logo />
        </Link>
        <UserDropdown />
      </nav>
      <main className="px-4 pt-24">
        <Outlet />
      </main>
    </>
  );
};
