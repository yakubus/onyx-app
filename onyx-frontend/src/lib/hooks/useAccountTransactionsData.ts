import { useMemo } from "react";
import { Transaction } from "@/lib/validation/transaction";
import {
  DEFAULT_MONTH_NUMBER,
  DEFAULT_YEAR_NUMBER,
} from "@/lib/constants/date";

export interface AccountTransactionsData {
  [x: number]: {
    balance: {
      [x: number]: {
        income: number;
        expenses: number;
      };
    };
  };
  availableDates: {
    [x: number]: number[];
  };
  selectedDateTransactions: Transaction[];
}

export const useAccountTransactionsData = (
  transactions: Transaction[],
  accMonth: string,
  accYear: string,
) => {
  const month = Number(accMonth);
  const year = Number(accYear);

  return useMemo(() => {
    const initialData: AccountTransactionsData = {
      [DEFAULT_YEAR_NUMBER]: {
        balance: {
          [DEFAULT_MONTH_NUMBER - 1]: {
            income: 0,
            expenses: 0,
          },
          [DEFAULT_MONTH_NUMBER]: {
            income: 0,
            expenses: 0,
          },
        },
      },
      availableDates: {
        [DEFAULT_YEAR_NUMBER]: [DEFAULT_MONTH_NUMBER - 1, DEFAULT_MONTH_NUMBER],
      },
      selectedDateTransactions: [],
    };

    transactions.forEach((transaction) => {
      const currentTransactionDate = new Date(transaction.transactedAt);
      const currentTransactionYear = currentTransactionDate.getFullYear();
      const currentTransactionMonth = currentTransactionDate.getMonth() + 1;
      const isExpense = transaction.amount.amount < 0;

      if (
        month === currentTransactionMonth &&
        year === currentTransactionYear
      ) {
        initialData.selectedDateTransactions.push(transaction);
      }

      if (!initialData[currentTransactionYear]) {
        initialData[currentTransactionYear] = {
          balance: {},
        };
      }

      if (
        !initialData[currentTransactionYear].balance[currentTransactionMonth]
      ) {
        initialData[currentTransactionYear].balance[currentTransactionMonth] = {
          income: 0,
          expenses: 0,
        };
      }

      const currentMonthBalance =
        initialData[currentTransactionYear].balance[currentTransactionMonth];

      if (isExpense) {
        currentMonthBalance.expenses -= transaction.amount.amount;
      } else {
        currentMonthBalance.income += transaction.amount.amount;
      }

      if (!initialData.availableDates[currentTransactionYear]) {
        initialData.availableDates[currentTransactionYear] = [];
      }

      const currentAvailableDates =
        initialData.availableDates[currentTransactionYear];
      if (!currentAvailableDates.includes(currentTransactionMonth)) {
        currentAvailableDates.push(currentTransactionMonth);
      }
    });

    Object.values(initialData.availableDates).forEach((dates) =>
      dates.sort((a, b) => a - b),
    );

    return initialData;
  }, [transactions, month, year]);
};
