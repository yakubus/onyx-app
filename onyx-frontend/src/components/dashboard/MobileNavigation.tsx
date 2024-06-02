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

interface NavLink {
  readonly href: string;
  readonly label: string;
}

interface MobileNavigationProps {
  navLinks: readonly NavLink[];
}

const MobileNavigation: FC<MobileNavigationProps> = ({ navLinks }) => {
  const [isNavOpen, setIsNavOpen] = useState(false);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const onLinkClick = (href: string) => {
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
        className="bg-primaryDark pr-0 text-primaryDark-foreground"
      >
        <SheetHeader>
          <SheetTitle className="flex justify-center text-center">
            <Logo />
          </SheetTitle>
        </SheetHeader>
        <div className="flex flex-col space-y-8 py-12">
          {navLinks.map(({ label, href }) => (
            <Button
              key={label}
              onClick={() => onLinkClick(href)}
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
      </SheetContent>
    </Sheet>
  );
};

export default MobileNavigation;
