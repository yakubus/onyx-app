import { Link, Outlet, createFileRoute } from "@tanstack/react-router";

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
});

function Layout() {
  const isDesktop = useMediaQuery("(min-width: 1024px)");

  if (!isDesktop) return <MobileLayout />;

  return (
    <div className="flex h-screen flex-col bg-background">
      <div className="mx-auto w-full max-w-screen-2xl flex-grow border-r shadow-xl shadow-primaryDark">
        <div className="flex h-full">
          <aside className="h-full w-1/5 bg-primaryDark py-10 text-primaryDark-foreground">
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
          <div className="flex flex-grow flex-col">
            <nav className="px-16 py-8 text-end">
              <UserDropdown />
            </nav>
            <main className="flex-grow px-12 py-8">
              <Outlet />
            </main>
          </div>
        </div>
      </div>
    </div>
  );
}

const MobileLayout = () => {
  return (
    <>
      <nav className="fixed h-80 flex w-full items-center justify-between bg-primaryDark px-4 py-2 text-primaryDark-foreground md:px-8">
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
