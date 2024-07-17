import { FC, useState } from "react";
import { useLocation, useNavigate } from "@tanstack/react-router";

import {
  AlignJustify,
  AreaChart,
  Goal,
  HelpCircle,
  Undo2,
  Wallet,
} from "lucide-react";
import Logo from "@/components/Logo";
import AccountsLinksAccordion from "@/components/dashboard/AccountsLinksAccordion";
import { Button } from "@/components/ui/button";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";

import { Account } from "@/lib/validation/account";
import { cn } from "@/lib/utils";

interface MobileNavigationProps {
  linksAvailable: boolean;
  budgetId: string | undefined;
  accounts: Account[];
}

const MobileNavigation: FC<MobileNavigationProps> = ({
  linksAvailable,
  budgetId,
  accounts,
}) => {
  const [isNavOpen, setIsNavOpen] = useState(false);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const onBackToBudgetsClick = () => {
    navigate({ to: "/budget" });
    setIsNavOpen(false);
  };

  const onButgetsLinkClick = () => {
    navigate({
      to: budgetId ? "/budget/$budgetId" : "/budget",
      search: (prev) => prev,
      mask: { to: budgetId ? "/budget/$budgetId" : "/budget" },
    });
    setIsNavOpen(false);
  };

  const onAccountLinkClick = (id: string) => {
    navigate({
      to: `/budget/${budgetId}/accounts/${id}`,
      search: (prev) => prev,
      mask: `/budget/${budgetId}/accounts/${id}`,
    });
    setIsNavOpen(false);
  };

  const onBudgetLayoutLinkClick = (route: string) => {
    navigate({
      to: `/budget/${budgetId}/${route}`,
      search: (prev) => prev,
      mask: `/budget/${budgetId}/${route}`,
    });
    setIsNavOpen(false);
  };

  const onNoLayoutLinkClick = (path: string) => {
    navigate({
      to: path,
    });
    setIsNavOpen(false);
  };

  const isBudgetLinkSelected = (pathname: string) =>
    pathname === `/budget/${budgetId}` || pathname === "/budget";

  const isRouteLinkSelected = (route: string) => pathname.includes(route);

  return (
    <Sheet open={isNavOpen} onOpenChange={setIsNavOpen}>
      <SheetTrigger asChild>
        <Button variant="ghost">
          <AlignJustify />
        </Button>
      </SheetTrigger>
      <SheetContent
        side="left"
        className="flex h-full flex-col bg-primaryDark pr-0 text-primaryDark-foreground"
      >
        <SheetHeader>
          <SheetTitle className="flex justify-center text-center">
            <Logo />
          </SheetTitle>
        </SheetHeader>
        <div className="flex flex-grow flex-col justify-between overflow-y-auto py-12">
          <div className="flex flex-col space-y-4">
            {budgetId && (
              <Button
                onClick={onBackToBudgetsClick}
                size="lg"
                variant="primaryDark"
                className="h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground"
              >
                <Undo2 />
                <span>Budgets</span>
              </Button>
            )}
            <Button
              onClick={onButgetsLinkClick}
              size="lg"
              variant="primaryDark"
              className={cn(
                "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                isBudgetLinkSelected(pathname) &&
                  "bg-background text-foreground",
              )}
            >
              <Wallet />
              <span>{budgetId ? "Budget" : "Budgets"}</span>
            </Button>

            {linksAvailable && (
              <>
                <AccountsLinksAccordion
                  accountsLength={accounts.length}
                  budgetId={budgetId!}
                >
                  {accounts.map((a) => (
                    <Button
                      key={a.id}
                      onClick={() => onAccountLinkClick(a.id)}
                      size="lg"
                      variant="primaryDark"
                      className={cn(
                        "justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                        pathname.includes(a.id) &&
                          "bg-background text-foreground",
                      )}
                    >
                      <span>{a.name}</span>
                    </Button>
                  ))}
                </AccountsLinksAccordion>
                <Button
                  onClick={() => onBudgetLayoutLinkClick("statistics")}
                  size="lg"
                  variant="primaryDark"
                  className={cn(
                    "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                    isRouteLinkSelected("statistics") &&
                      "bg-background text-foreground",
                  )}
                >
                  <AreaChart />
                  <span>Statistics</span>
                </Button>

                <Button
                  onClick={() => onBudgetLayoutLinkClick("goals")}
                  size="lg"
                  variant="primaryDark"
                  className={cn(
                    "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                    isRouteLinkSelected("goals") &&
                      "bg-background text-foreground",
                  )}
                >
                  <Goal />
                  <span>Goals</span>
                </Button>
              </>
            )}
          </div>
          <div className="flex flex-col space-y-4">
            <Button
              onClick={() => onNoLayoutLinkClick("/help")}
              size="lg"
              variant="primaryDark"
              className={cn(
                "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                isRouteLinkSelected("help") && "bg-background text-foreground",
              )}
            >
              <HelpCircle />
              <span>Help</span>
            </Button>
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
};

export default MobileNavigation;
