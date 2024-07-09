import { FC, useMemo } from "react";

import CreateTransactionButton from "@/components/dashboard/accounts/CreateTransactionButton";

import { type Account } from "@/lib/validation/account";
import { type Category } from "@/lib/validation/category";
import { type Transaction } from "@/lib/validation/transaction";

interface TransactionsTableProps {
  data: Transaction[] | undefined;
  account: Account;
  categories: Category[];
}

const TransactionsTable: FC<TransactionsTableProps> = ({
  data,
  account,
  categories,
}) => {
  const selectableCategories = useMemo(() => {
    if (!categories.length) return null;
    return categories
      .filter((c) => c.subcategories.length > 0)
      .map((c) => ({
        label: c.name,
        value: c.id,
        subcategories: c.subcategories.map((s) => ({
          label: s.name,
          value: s.id,
        })),
      }));
  }, [categories]);

  return (
    <div className="space-y-10 p-10">
      <div>
        {selectableCategories && (
          <CreateTransactionButton
            account={account}
            selectableCategories={selectableCategories}
          />
        )}
      </div>
      {!data || data.length === 0 ? (
        <h2 className="font-bold">
          No added transactions for this account in selected date.
        </h2>
      ) : (
        data.map((transaction) => (
          <div key={transaction.id}>{transaction.amount.amount}</div>
        ))
      )}
    </div>
  );
};

export default TransactionsTable;
