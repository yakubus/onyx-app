import {
  Link,
  Outlet,
  createLazyFileRoute,
  useLocation,
} from "@tanstack/react-router";

import { Undo2 } from "lucide-react";
import Logo from "@/components/Logo";
import UserDropdown from "@/components/dashboard/UserDropdown";
import MobileNavigation from "@/components/dashboard/MobileNavigation";

import { useMediaQuery } from "@/lib/hooks/useMediaQuery";
import { BUDGET_LINKS, SIDEBAR_BOTTOM_LINKS } from "@/lib/constants/links";
import { cn } from "@/lib/utils";
import { useSingleBudgetLoadingState } from "@/lib/hooks/useSingleBudgetLoadingState";

export const Route = createLazyFileRoute("/_dashboard-layout")({
  component: Layout,
});

function Layout() {
  const isDesktop = useMediaQuery("(min-width: 1024px)");
  const { pathname } = useLocation();
  const isBudgetSelected = pathname.startsWith("/budget/");
  const budgetId = pathname.split("/")[2];
  const { isError: isSingleBudgetLoadingError } =
    useSingleBudgetLoadingState(budgetId);

  const { icon: BudgetIcon } = BUDGET_LINKS[0];

  if (!isDesktop)
    return (
      <MobileLayout
        isBudgetSelected={isBudgetSelected}
        isSingleBudgetLoadingError={isSingleBudgetLoadingError}
      />
    );

  return (
    <div className="flex h-screen flex-col bg-background">
      <div className="mx-auto w-full max-w-screen-2xl flex-grow border-r shadow-xl shadow-primaryDark">
        <div className="grid h-full w-full grid-cols-5">
          <aside className="col-span-1 flex h-full flex-col bg-primaryDark py-10 text-primaryDark-foreground">
            <Link to="/budget" className="mx-auto">
              <Logo />
            </Link>
            <div className="flex flex-grow flex-col space-y-10 py-16 pl-10">
              {isBudgetSelected && (
                <Link
                  to="/budget"
                  className={cn(
                    "grid grid-rows-[0fr] items-center rounded-l-full pl-9 text-sm font-semibold tracking-wide transition-all duration-500 ease-in-out hover:bg-background hover:text-foreground",
                    isBudgetSelected && "grid-rows-[1fr] py-4",
                  )}
                  preload="intent"
                >
                  <span className="flex items-center space-x-4 overflow-hidden">
                    <Undo2 />
                    <span>Budgets</span>
                  </span>
                </Link>
              )}
              <Link
                to={isBudgetSelected ? "/budget/$budgetId" : "/budget"}
                search={(prev) => prev}
                mask={{
                  to: isBudgetSelected ? "/budget/$budgetId" : "/budget",
                }}
                className="rounded-l-full py-4 pl-9 text-sm font-semibold transition-all duration-500 hover:bg-background hover:text-foreground"
                activeProps={{
                  className: "bg-background text-foreground",
                }}
                preload="intent"
                activeOptions={{ exact: true }}
              >
                <span className="flex items-center space-x-4 overflow-hidden">
                  <BudgetIcon />
                  <span>{isBudgetSelected ? "Budget" : "Budgets"}</span>
                </span>
              </Link>
              {!isSingleBudgetLoadingError &&
                BUDGET_LINKS.slice(1).map(({ label, href, icon: Icon }) => (
                  <Link
                    key={label}
                    to={`/budget/$budgetId/${href}`}
                    search={(prev) => prev}
                    mask={{ to: `/budget/$budgetId/${href}` }}
                    className={cn(
                      "grid grid-rows-[0fr] rounded-l-full pl-9 text-sm font-semibold transition-all duration-500 ease-in-out hover:bg-background hover:text-foreground",
                      isBudgetSelected && "grid-rows-[1fr] py-4",
                    )}
                    activeProps={{
                      className: "bg-background text-foreground",
                    }}
                    preload="intent"
                  >
                    <span className="flex items-center space-x-4 overflow-hidden">
                      <Icon />
                      <span>{label}</span>
                    </span>
                  </Link>
                ))}
            </div>
            <div className="flex flex-col space-y-4 py-16 pl-10">
              {SIDEBAR_BOTTOM_LINKS.map(({ label, href, icon: Icon }) => (
                <Link
                  key={label}
                  to={href}
                  className="rounded-l-full py-4 pl-9 text-sm font-semibold transition-all duration-300 hover:bg-background hover:text-foreground"
                  activeProps={{
                    className: "bg-background text-foreground",
                  }}
                  preload="intent"
                >
                  <span className="flex items-center space-x-4 overflow-hidden">
                    <Icon />
                    <span>{label}</span>
                  </span>
                </Link>
              ))}
            </div>
          </aside>
          <div className="fixed left-0 top-0 w-full">
            <nav className="mx-auto max-w-screen-2xl px-28 py-10 text-end">
              <UserDropdown />
            </nav>
          </div>
          <main className="col-span-4 h-screen pb-4 pt-28">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}

const MobileLayout = ({
  isBudgetSelected,
  isSingleBudgetLoadingError,
}: {
  isBudgetSelected: boolean;
  isSingleBudgetLoadingError: boolean;
}) => {
  return (
    <>
      <nav className="fixed z-50 flex w-full items-center justify-between bg-primaryDark px-4 py-2 text-primaryDark-foreground md:px-8">
        <MobileNavigation
          isBudgetSelected={isBudgetSelected}
          isSingleBudgetLoadingError={isSingleBudgetLoadingError}
        />
        <Link to="/budget">
          <Logo />
        </Link>
        <UserDropdown />
      </nav>
      <main className="pb-12 pt-24">
        <Outlet />
      </main>
    </>
  );
};
