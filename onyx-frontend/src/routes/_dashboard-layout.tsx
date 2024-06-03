import {
  Link,
  Outlet,
  createFileRoute,
  redirect,
} from "@tanstack/react-router";

import { useMediaQuery } from "@/lib/hooks/useMediaQuery";

import Logo from "@/components/Logo";
import MobileNavigation from "@/components/dashboard/MobileNavigation";
import UserDropdown from "@/components/dashboard/UserDropdown";

const navLinks = [
  { href: "/budget", label: "Budget" },
  { href: "/accounts", label: "Accounts" },
  { href: "/statistics", label: "Statistics" },
  { href: "/goals", label: "Goals" },
  { href: "/help", label: "Help" },
] as const;

export const Route = createFileRoute("/_dashboard-layout")({
  component: Layout,
  beforeLoad: ({ context: { user }, location }) => {
    if (!user || !user.id) {
      throw redirect({
        to: "/",
        search: {
          redirect: location.href,
        },
      });
    }
  },
});

function Layout() {
  const isDesktop = useMediaQuery("(min-width: 1024px)");

  if (!isDesktop) return <MobileLayout />;

  return (
    <div className="flex h-screen flex-col bg-background">
      <div className="mx-auto w-full max-w-screen-2xl flex-grow border-r shadow-xl shadow-primaryDark">
        <div className="grid h-full w-full grid-cols-5">
          <aside className="col-span-1 h-full bg-primaryDark py-10 text-primaryDark-foreground">
            <Link to="/budget">
              <Logo />
            </Link>
            <div>
              <div className="flex flex-col space-y-10 py-16 pl-10">
                {navLinks.map(({ label, href }) => (
                  <Link
                    key={label}
                    to={href}
                    className="rounded-l-full py-4 pl-9 transition-all duration-300 hover:bg-background hover:text-foreground"
                    activeProps={{
                      className: "bg-background text-foreground",
                    }}
                  >
                    {label}
                  </Link>
                ))}
              </div>
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

const MobileLayout = () => {
  return (
    <>
      <nav className="fixed z-50 flex w-full items-center justify-between bg-primaryDark px-4 py-2 text-primaryDark-foreground md:px-8">
        <MobileNavigation navLinks={navLinks} />
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
