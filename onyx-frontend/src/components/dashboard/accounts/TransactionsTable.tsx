import { FC } from "react";
import { Transaction } from "@/lib/validation/transaction";

interface TransactionsTableProps {
  data: Transaction[] | undefined;
}

const TransactionsTable: FC<TransactionsTableProps> = ({ data }) => {
  if (!data || data.length === 0) {
    return (
      <h2 className="font-bold">
        No added transactions for this account in selected date.
      </h2>
    );
  }

  return (
    <div>
      {data.map((transaction) => (
        <div>{JSON.stringify(transaction)}</div>
      ))}
    </div>
  );
};

export default TransactionsTable;
