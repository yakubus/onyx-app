import { FC, useState } from "react";
import { useLocation, useNavigate } from "@tanstack/react-router";

import { cn } from "@/lib/utils";

import { AlignJustify } from "lucide-react";
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
  selectedBudget: string | undefined;
}

const MobileNavigation: FC<MobileNavigationProps> = ({ selectedBudget }) => {
  const [isNavOpen, setIsNavOpen] = useState(false);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const onBudgetLinkClick = (href: string) => {
    navigate({ to: href, search: (prev) => prev, mask: { to: `/${href}` } });
    setIsNavOpen(false);
  };

  const onButgetsLinkClick = () => {
    navigate({
      to: selectedBudget ? `/budget/${selectedBudget}` : "/budget",
      search: (prev) => prev,
      mask: selectedBudget && { to: `/budget/${selectedBudget}` },
    });
    setIsNavOpen(false);
  };

  const onBottomLinkClick = (href: string) => {
    navigate({ to: href });
    setIsNavOpen(false);
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
            <Button
              onClick={onButgetsLinkClick}
              size="lg"
              variant="primaryDark"
              className={cn(
                "text-md h-14 justify-start rounded-l-full rounded-r-none transition-colors duration-300 hover:bg-background hover:text-foreground",
                pathname.startsWith("/budget") &&
                  "bg-background text-foreground",
              )}
            >
              Budget
            </Button>
            {BUDGET_LINKS.map(({ label, href }) => (
              <Button
                key={label}
                onClick={() => onBudgetLinkClick(href)}
                size="lg"
                variant="primaryDark"
                className={cn(
                  "text-md h-14 justify-start rounded-l-full rounded-r-none transition-colors duration-300 hover:bg-background hover:text-foreground",
                  pathname === href && "bg-background text-foreground",
                  !selectedBudget && "hidden",
                )}
              >
                {label}
              </Button>
            ))}
          </div>
          <div className="flex flex-col space-y-4">
            {SIDEBAR_BOTTOM_LINKS.map(({ label, href }) => (
              <Button
                key={label}
                onClick={() => onBottomLinkClick(href)}
                size="lg"
                variant="primaryDark"
                className={cn(
                  "text-md h-14 justify-start rounded-l-full rounded-r-none transition-colors duration-300 hover:bg-background hover:text-foreground",
                  pathname === href && "bg-background text-foreground",
                )}
              >
                {label}
              </Button>
            ))}
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
};

export default MobileNavigation;
