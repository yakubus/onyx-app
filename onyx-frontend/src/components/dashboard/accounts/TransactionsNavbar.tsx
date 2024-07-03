import { FC, useMemo } from "react";
import CreateTransactionButton from "./CreateTransactionButton";
import { Account } from "@/lib/validation/account";
import { useParams } from "@tanstack/react-router";
import { useQueryClient } from "@tanstack/react-query";
import { getCategoriesQueryOptions } from "@/lib/api/category";

interface TransactionsNavbarProps {
  account: Account;
}

const TransactionsNavbar: FC<TransactionsNavbarProps> = ({ account }) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });

  const categories = queryClient.getQueryData(
    getCategoriesQueryOptions(budgetId).queryKey,
  );
  const selectableSubcategories = useMemo(() => {
    if (!categories) return null;
    return categories[0].subcategories.map((s) => ({
      label: s.name,
      value: s.id,
    }));
  }, [categories]);

  return (
    <div>
      {selectableSubcategories && (
        <CreateTransactionButton
          account={account}
          selectableSubcategories={selectableSubcategories}
        />
      )}
    </div>
  );
};

export default TransactionsNavbar;
