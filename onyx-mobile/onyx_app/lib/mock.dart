import 'models/money.dart';
import 'views/accounts/accounts.dart';

class Mocks {
  static List<Account> accountsMock = [
    Account(
      id: '1',
      name: 'Account 1',
      balance: Money(amount: 100, currency: 'USD'),
      type: 'Checking',
    ),
    Account(
      id: '2',
      name: 'Account 2',
      balance: Money(amount: 200, currency: 'PLN'),
      type: 'Savings',
    ),
    Account(
      id: '3',
      name: 'Account 3',
      balance: Money(amount: 300, currency: 'PLN'),
      type: 'Checking',
    ),
  ];
}
