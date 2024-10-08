import { Column, ColumnDef, Table } from "@tanstack/react-table";
import { format } from "date-fns";

import { ArrowUpDown, ArrowUp, ArrowDown, Ellipsis } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";

import { Transaction } from "@/lib/validation/transaction";
import { capitalize, cn, getFormattedCurrency } from "@/lib/utils";

export const columns: ColumnDef<Transaction>[] = [
  {
    id: "select",
    header: ({ table }) => (
      <div className="flex items-center overflow-hidden">
        <Checkbox
          checked={
            table.getIsAllPageRowsSelected() ||
            (table.getIsSomePageRowsSelected() && "indeterminate")
          }
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label="Select all"
        />
      </div>
    ),
    cell: ({ row }) => (
      <div className="flex items-center overflow-hidden">
        <Checkbox
          checked={row.getIsSelected()}
          onCheckedChange={(value) => row.toggleSelected(!!value)}
          aria-label="Select row"
        />
      </div>
    ),
  },
  {
    accessorFn: (row) => {
      const date = new Date(row.transactedAt);
      const formattedDate = format(date, "PP");
      return formattedDate;
    },
    id: "date",
    header: ({ column, table }) => (
      <SortButton column={column} label="Date" table={table} />
    ),
    cell: ({ row }) => {
      return <div className="pl-4">{row.getValue("date")}</div>;
    },
    sortDescFirst: true,
    sortingFn: "alphanumeric",
  },
  {
    accessorKey: "counterparty.name",
    id: "counterparty",
    header: ({ column, table }) => (
      <SortButton column={column} label="Counterparty" table={table} />
    ),
    cell: ({ row }) => (
      <div className="pl-4">{capitalize(row.getValue("counterparty"))}</div>
    ),
  },
  {
    accessorKey: "subcategory.name",
    id: "subcategory",
    header: ({ column, table }) => (
      <SortButton column={column} label="Subcategory" table={table} />
    ),
    cell: ({ row }) => {
      const subcategoryName = row.getValue<string>("subcategory") || "N/A";

      return <div className="pl-4">{subcategoryName}</div>;
    },
    sortUndefined: "last",
  },
  {
    accessorKey: "amount.amount",
    id: "amount",
    header: ({ column, table }) => (
      <div className="text-right">
        <SortButton column={column} label="Amount" table={table} />
      </div>
    ),
    cell: ({ row }) => {
      const amount = row.getValue("amount") as number;
      const currency = row.original.amount.currency;
      const accCurrency = row.original.account.balance.currency;
      const formatted = getFormattedCurrency(amount, currency);

      return (
        <div
          className={cn(
            "flex justify-end space-x-1 pr-5 font-semibold",
            amount < 0 ? "text-destructive" : "text-primary",
          )}
        >
          <span>
            {accCurrency === currency ? (
              formatted
            ) : (
              <Ellipsis className="animate-pulse" />
            )}
          </span>
        </div>
      );
    },
  },
];

const SortButton = ({
  column,
  label,
  table,
}: {
  column: Column<Transaction>;
  label: string;
  table: Table<Transaction>;
}) => {
  const isSorted = column.getIsSorted();

  const hasValues = table.getRowModel().rows.some((row) => {
    const value = row.getValue(column.id);
    return value !== null && value !== undefined;
  });

  return (
    <Button
      variant="ghost"
      onClick={() => column.toggleSorting(undefined, true)}
      disabled={!hasValues}
    >
      <span className="inline-flex font-semibold tracking-wide">{label}</span>
      {isSorted === "asc" && (
        <ArrowUp className="ml-2 inline-flex h-4 w-4 shrink-0" />
      )}
      {isSorted === "desc" && (
        <ArrowDown className="ml-2 inline-flex h-4 w-4 shrink-0" />
      )}
      {isSorted === false && (
        <ArrowUpDown className="ml-2 inline-flex h-4 w-4 shrink-0" />
      )}
    </Button>
  );
};
