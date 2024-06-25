import {
  BudgetIcon,
  AccountsIcon,
  StatisticsIcon,
  GoalsIcon,
  HelpIcon,
} from "@/lib/constants/icons";

export const BUDGET_LINKS = [
  { href: "/budget", label: "Budget", icon: BudgetIcon },
  { href: "/accounts", label: "Accounts", icon: AccountsIcon },
  { href: "/statistics", label: "Statistics", icon: StatisticsIcon },
  { href: "/goals", label: "Goals", icon: GoalsIcon },
] as const;

export const SIDEBAR_BOTTOM_LINKS = [
  { href: "/help", label: "Help", icon: HelpIcon },
] as const;
