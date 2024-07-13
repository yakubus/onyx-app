import { FC, useMemo, useState } from "react";
import {
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
} from "@tanstack/react-table";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Input } from "../../ui/input";
import { Transaction } from "@/lib/validation/transaction";
import { Account } from "@/lib/validation/account";
import { Category } from "@/lib/validation/category";
import { columns } from "./TransactionsTableColumns";
import CreateTransactionButton from "./CreateTransactionButton";
import { useDebounce } from "@/lib/hooks/useDebounce";
import DeleteTransactionsButton from "./DeleteTransactionsButton";
import { useMediaQuery } from "@/lib/hooks/useMediaQuery";
import { Minus, Plus } from "lucide-react";
import { cn } from "@/lib/utils";
import CreateTransactionTableForm from "./CreateTransactionTableForm";

interface TransactionsTable {
  transactions: Transaction[];
  selectedAccount: Account;
  categories: Category[];
}

const TransactionsTable: FC<TransactionsTable> = ({
  transactions,
  selectedAccount,
  categories,
}) => {
  const isLargeDevice = useMediaQuery("(min-width: 1024px)");
  const [isCreateFormVisible, setIsCreateFormVisible] = useState(false);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [pagination, setPagination] = useState({
    pageIndex: 0,
    pageSize: 8,
  });
  const [rowSelection, setRowSelection] = useState({});
  const [globalFilter, setGlobalFilter] = useState<string>("");
  const debouncedGlobalFilter = useDebounce(globalFilter, 500);

  const table = useReactTable({
    data: transactions,
    columns,
    onSortingChange: setSorting,
    onPaginationChange: setPagination,
    onRowSelectionChange: setRowSelection,
    onGlobalFilterChange: setGlobalFilter,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    autoResetPageIndex: false,
    state: {
      sorting,
      pagination,
      globalFilter: debouncedGlobalFilter,
      rowSelection,
    },
  });

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

  if (!selectableCategories || !selectableCategories.length)
    return (
      <div className="w-full pt-20 text-center">
        <h2 className="text-lg font-semibold">
          Please create some categories and subcategories first, then add
          transactions.
        </h2>
      </div>
    );

  return (
    <div className="pt-7">
      <div className="flex flex-col justify-between space-y-2 py-4 md:flex-row md:space-y-0">
        <div className="flex flex-col space-y-2 md:flex-row md:space-x-2 md:space-y-0">
          {isLargeDevice ? (
            <Button
              variant="outline"
              className="space-x-2"
              onClick={() => setIsCreateFormVisible(!isCreateFormVisible)}
            >
              {isCreateFormVisible ? (
                <Minus className="inline-flex size-5 flex-shrink-0" />
              ) : (
                <Plus className="inline-flex size-5 flex-shrink-0" />
              )}
              <span className="inline-flex">
                {isCreateFormVisible ? "Hide" : "Create"}
              </span>
            </Button>
          ) : (
            <CreateTransactionButton
              account={selectedAccount}
              selectableCategories={selectableCategories}
            />
          )}
          {table.getFilteredSelectedRowModel().rows.length > 0 && (
            <DeleteTransactionsButton
              rows={table.getFilteredSelectedRowModel().rows}
            />
          )}
        </div>

        <Input
          placeholder="Search..."
          value={globalFilter}
          onChange={(e) => setGlobalFilter(e.target.value)}
          className="w-full md:max-w-xs"
        />
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id}>
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext(),
                          )}
                    </TableHead>
                  );
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {isLargeDevice && (
              <TableRow
                className={cn(!isCreateFormVisible && "border-none py-0")}
              >
                <TableCell colSpan={columns.length} className="py-0">
                  <div
                    className={cn(
                      "grid grid-rows-[0fr] transition-all duration-300",
                      isCreateFormVisible && "grid-rows-[1fr] py-2",
                    )}
                  >
                    <CreateTransactionTableForm />
                  </div>
                </TableCell>
              </TableRow>
            )}
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {flexRender(
                        cell.column.columnDef.cell,
                        cell.getContext(),
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-24 text-center"
                >
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex items-center justify-between py-4">
        <div className="pl-4 text-sm text-muted-foreground">
          {table.getFilteredSelectedRowModel().rows.length} of{" "}
          {table.getFilteredRowModel().rows.length} row(s) selected.
        </div>
        <div className="space-x-2">
          <Button
            variant="outline"
            size="sm"
            onClick={() => table.previousPage()}
            disabled={!table.getCanPreviousPage()}
          >
            Previous
          </Button>
          <Button
            variant="outline"
            size="sm"
            onClick={() => table.nextPage()}
            disabled={!table.getCanNextPage()}
          >
            Next
          </Button>
        </div>
      </div>
    </div>
  );
};

export default TransactionsTable;
