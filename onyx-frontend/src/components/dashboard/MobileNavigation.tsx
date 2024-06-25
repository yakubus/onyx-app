import { FC, useState } from "react";
import { useLocation, useNavigate } from "@tanstack/react-router";

import { cn } from "@/lib/utils";

import { AlignJustify, Undo2 } from "lucide-react";
import Logo from "@/components/Logo";
import { Button } from "@/components/ui/button";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";

import { BUDGET_LINKS, SIDEBAR_BOTTOM_LINKS } from "@/lib/constants/links";
interface MobileNavigationProps {
  isBudgetSelected: boolean;
  isSingleBudgetLoadingError: boolean;
}

const MobileNavigation: FC<MobileNavigationProps> = ({
  isBudgetSelected,
  isSingleBudgetLoadingError,
}) => {
  const [isNavOpen, setIsNavOpen] = useState(false);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const onBudgetLinkClick = (href: string) => {
    navigate({
      to: `/budget/$budgetId/${href}`,
      search: (prev) => prev,
      mask: { to: `/budget/$budgetId/${href}` },
    });
    setIsNavOpen(false);
  };

  const onButgetsLinkClick = () => {
    navigate({
      to: isBudgetSelected ? "/budget/$budgetId" : "/budget",
      search: (prev) => prev,
      mask: { to: isBudgetSelected ? "/budget/$budgetId" : "/budget" },
    });
    setIsNavOpen(false);
  };

  const onBottomLinkClick = (href: string) => {
    navigate({ to: href });
    setIsNavOpen(false);
  };

  const onBackToBudgetsClick = () => {
    navigate({ to: "/budget" });
    setIsNavOpen(false);
  };

  const { icon: BudgetIcon } = BUDGET_LINKS[0];

  const isBudgetLinkSelected = (pathname: string) => {
    return (
      pathname.startsWith("/budget") &&
      !BUDGET_LINKS.slice(1).some((link) => pathname.includes(link.href))
    );
  };

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
        <div className="flex flex-grow flex-col justify-between py-12">
          <div className="flex flex-col space-y-8">
            {isBudgetSelected && (
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
              <BudgetIcon />
              <span>{isBudgetSelected ? "Budget" : "Budgets"}</span>
            </Button>
            {!isSingleBudgetLoadingError &&
              BUDGET_LINKS.slice(1).map(({ label, href, icon: Icon }) => (
                <Button
                  key={label}
                  onClick={() => onBudgetLinkClick(href)}
                  size="lg"
                  variant="primaryDark"
                  className={cn(
                    "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                    pathname.endsWith(href) && "bg-background text-foreground",
                    !isBudgetSelected && "hidden",
                  )}
                >
                  <Icon />
                  <span>{label}</span>
                </Button>
              ))}
          </div>
          <div className="flex flex-col space-y-4">
            {SIDEBAR_BOTTOM_LINKS.map(({ label, href, icon: Icon }) => (
              <Button
                key={label}
                onClick={() => onBottomLinkClick(href)}
                size="lg"
                variant="primaryDark"
                className={cn(
                  "h-14 justify-start space-x-4 rounded-l-full rounded-r-none text-sm font-semibold tracking-widest transition-colors duration-300 hover:bg-background hover:text-foreground",
                  pathname === href && "bg-background text-foreground",
                )}
              >
                <Icon />
                <span>{label}</span>
              </Button>
            ))}
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
};

export default MobileNavigation;
