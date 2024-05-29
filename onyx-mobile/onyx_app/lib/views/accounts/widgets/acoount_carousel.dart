import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:carousel_slider_plus/carousel_slider_plus.dart';
import 'package:onyx_app/views/accounts/account_repo.dart';
import 'package:onyx_app/views/accounts/accounts.dart';

class AccountsCarousel extends HookConsumerWidget {
  const AccountsCarousel({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final accountRepo = ref.watch(accountRepoProvider);

    return FutureBuilder<List<Account>>(
      future: accountRepo.getAccounts(),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const CircularProgressIndicator();
        } else if (snapshot.hasError) {
          return Text('Error: ${snapshot.error}');
        } else {
          return CarouselSlider.builder(
            itemCount: snapshot.data!.length,
            itemBuilder: (context, index, realIdx) {
              final account = snapshot.data![index];
              return ListTile(
                title: Text(account.name),
                subtitle: Text(
                    '${account.balance.amount} ${account.balance.currency}'),
              );
            },
            options: CarouselOptions(
              autoPlay: true,
              enlargeCenterPage: true,
            ),
          );
        }
      },
    );
  }
}

class AccountCarousel extends HookConsumerWidget {
  const AccountCarousel({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final accountRepo = ref.watch(accountRepoProvider);
    final accounts = useState<List<Account>>([]);
    final currentIndex = useState(0);

    useEffect(() {
      Future<void> loadAccounts() async {
        try {
          accounts.value = await accountRepo.getAccounts();
        } catch (e) {
          print('Wystąpił błąd: $e');
        }
      }

      loadAccounts();
      return null;
    }, const []);

    return accounts.value.isEmpty
        ? const CircularProgressIndicator()
        : Column(
            children: [
              Text('Wybrane konto: ${accounts.value[currentIndex.value].name}'),
              const SizedBox(height: 20),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: accounts.value.map((account) {
                  int index = accounts.value.indexOf(account);
                  return IconButton(
                    icon: Icon(
                      Icons.arrow_right,
                      color: currentIndex.value == index
                          ? Colors.blue
                          : Colors.grey,
                    ),
                    onPressed: () {
                      currentIndex.value = index;
                    },
                  );
                }).toList(),
              ),
            ],
          );
  }
}
