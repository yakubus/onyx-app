import {
  Link,
  Outlet,
  createLazyFileRoute,
  useLocation,
} from "@tanstack/react-router";

import { AreaChart, Goal, HelpCircle, Undo2, Wallet } from "lucide-react";
import Logo from "@/components/Logo";
import UserDropdown from "@/components/dashboard/UserDropdown";
import MobileNavigation from "@/components/dashboard/MobileNavigation";
import AccountsLinksAccordion from "@/components/dashboard/AccountsLinksAccordion";

import { useMediaQuery } from "@/lib/hooks/useMediaQuery";
import { cn } from "@/lib/utils";
import { useSingleBudgetLoadingState } from "@/lib/hooks/useSingleBudgetLoadingState";
import { useQuery } from "@tanstack/react-query";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { Account } from "@/lib/validation/account";
import { SingleBudgetPageSearchParams } from "@/lib/validation/searchParams";

export const Route = createLazyFileRoute("/_dashboard-layout")({
  component: Layout,
});

function Layout() {
  const isDesktop = useMediaQuery("(min-width: 1280px)");
  const { pathname } = useLocation();
  const isBudgetSelected = pathname.startsWith("/budget/");
  const budgetId = pathname.split("/")[2] as string | undefined;
  const { isError: isSingleBudgetLoadingError } =
    useSingleBudgetLoadingState(budgetId);
  const linksAvailable = isBudgetSelected && !isSingleBudgetLoadingError;

  const { data: accounts = [] } = useQuery({
    ...getAccountsQueryOptions(budgetId!),
    enabled: false,
  });

  if (!isDesktop)
    return (
      <MobileLayout
        linksAvailable={linksAvailable}
        budgetId={budgetId}
        accounts={accounts}
      />
    );

  return (
    <div className="flex h-screen bg-background">
      <aside className="sticky left-0 flex h-full w-[250px] flex-col justify-between bg-primaryDark py-10 text-primaryDark-foreground lg:w-[300px]">
        <Link to="/budget" className="mx-auto">
          <Logo />
        </Link>

        <div className="flex flex-grow flex-col space-y-4 py-16 pl-10">
          <Link
            to="/budget"
            className={cn(
              "grid grid-rows-[0fr] rounded-l-full pl-9 text-sm font-semibold transition-all duration-500 ease-in-out hover:bg-accent hover:text-foreground",
              isBudgetSelected && "grid-rows-[1fr] py-4",
            )}
            preload="intent"
          >
            <span className="space-x-4 overflow-hidden">
              <Undo2 className="inline-flex size-6 shrink-0" />
              <span className="inline-flex text-sm font-semibold tracking-wide">
                Budgets
              </span>
            </span>
          </Link>

          <Link
            to={isBudgetSelected ? "/budget/$budgetId" : "/budget"}
            search={(prev) => prev}
            mask={{
              to: isBudgetSelected ? "/budget/$budgetId" : "/budget",
            }}
            className="rounded-l-full py-4 pl-9 transition-all duration-500 hover:bg-accent hover:text-foreground"
            activeProps={{
              className: "bg-background text-foreground",
            }}
            preload="intent"
            activeOptions={{ exact: true }}
          >
            <span className="space-x-4 overflow-hidden">
              <Wallet className="inline-flex size-6 shrink-0" />
              <span className="inline-flex text-sm font-semibold tracking-wide">
                {isBudgetSelected ? "Budget" : "Budgets"}
              </span>
            </span>
          </Link>

          <div
            className={cn(
              "grid grid-rows-[0fr] transition-all duration-500 ease-in-out",
              linksAvailable && "grid-rows-[1fr]",
            )}
          >
            <div className="overflow-hidden">
              <AccountsLinksAccordion
                budgetId={budgetId!}
                accountsLength={accounts.length}
              >
                {accounts.map((account) => (
                  <Link
                    key={account.id}
                    to={`/budget/${budgetId}/accounts/${account.id}`}
                    params={{ budgetId: budgetId!, accountId: account.id }}
                    search={(prev) => prev as SingleBudgetPageSearchParams}
                    mask={{
                      to: `/budget/${budgetId}/accounts/${account.id}`,
                    }}
                    className="rounded-l-full py-4 pl-9 text-sm font-semibold transition-all duration-300 hover:bg-accent hover:text-foreground"
                    activeProps={{
                      className: "bg-background text-foreground",
                    }}
                    preload="intent"
                  >
                    {account.name}
                  </Link>
                ))}
              </AccountsLinksAccordion>
            </div>
          </div>

          <Link
            to="/budget/$budgetId/statistics"
            params={{ budgetId: budgetId! }}
            search={(prev) => prev}
            mask={{
              to: `/budget/${budgetId}/statistics`,
            }}
            className={cn(
              "grid grid-rows-[0fr] rounded-l-full pl-9 text-sm font-semibold transition-all duration-500 ease-in-out hover:bg-accent hover:text-foreground",
              linksAvailable && "grid-rows-[1fr] py-4",
            )}
            activeProps={{
              className: "bg-background text-foreground",
            }}
            preload="intent"
            activeOptions={{ exact: true }}
          >
            <span className="space-x-4 overflow-hidden">
              <AreaChart className="inline-flex size-6 shrink-0" />
              <span className="inline-flex text-sm font-semibold tracking-wide">
                Statistics
              </span>
            </span>
          </Link>

          <Link
            to="/budget/$budgetId/goals"
            params={{ budgetId: budgetId! }}
            search={(prev) => prev}
            mask={{
              to: `/budget/${budgetId}/goals`,
            }}
            className={cn(
              "grid grid-rows-[0fr] rounded-l-full pl-9 transition-all duration-500 ease-in-out hover:bg-accent hover:text-foreground",
              linksAvailable && "grid-rows-[1fr] py-4",
            )}
            activeProps={{
              className: "bg-background text-foreground",
            }}
            preload="intent"
            activeOptions={{ exact: true }}
          >
            <span className="space-x-4 overflow-hidden">
              <Goal className="inline-flex size-6 shrink-0" />
              <span className="inline-flex text-sm font-semibold tracking-wide">
                Goals
              </span>
            </span>
          </Link>
        </div>

        <div className="flex flex-col space-y-4 py-4 pl-10">
          <Link
            to="/help"
            className="rounded-l-full py-4 pl-9 transition-all duration-500 ease-in-out hover:bg-accent hover:text-foreground"
            activeProps={{
              className: "bg-background text-foreground",
            }}
            preload="intent"
            activeOptions={{ exact: true }}
          >
            <span className="space-x-4 overflow-hidden">
              <HelpCircle className="inline-flex size-6 shrink-0" />
              <span className="inline-flex text-sm font-semibold tracking-wide">
                Help
              </span>
            </span>
          </Link>
        </div>
      </aside>

      <main className="mx-auto flex h-screen w-full max-w-screen-xl flex-col space-y-8 px-8 pb-4 pt-8">
        <nav className="text-end">
          <UserDropdown />
        </nav>
        <Outlet />
      </main>
    </div>
  );
}

const MobileLayout = ({
  linksAvailable,
  budgetId,
  accounts,
}: {
  linksAvailable: boolean;
  budgetId: string | undefined;
  accounts: Account[];
}) => {
  return (
    <>
      <nav className="fixed z-50 flex w-full items-center justify-between bg-primaryDark px-4 py-2 text-primaryDark-foreground md:px-8">
        <MobileNavigation
          linksAvailable={linksAvailable}
          budgetId={budgetId}
          accounts={accounts}
        />
        <Link to="/budget">
          <Logo />
        </Link>
        <UserDropdown />
      </nav>
      <main className="px-4 pb-12 pt-28 md:px-8">
        <Outlet />
      </main>
    </>
  );
};
