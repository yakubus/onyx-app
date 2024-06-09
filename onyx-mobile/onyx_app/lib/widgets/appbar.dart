import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/config.dart';
import 'package:onyx_app/widgets/main_menu/main_menu.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class DefaultAppBar extends HookConsumerWidget implements PreferredSizeWidget {
  final String title;
  const DefaultAppBar({
    Key? key,
    required this.title,
  }) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Padding(
      padding: const EdgeInsets.all(5),
      child: AppBar(
        backgroundColor: Theme.of(context).colorScheme.primary,
        title: Text(title),
        leading: IconButton(
          icon: const Icon(Icons.menu),
          onPressed: () => showShadSheet(
            side: ShadSheetSide.left,
            context: context,
            builder: (context) => const MainMenu(),
          ),
        ),
        actions: [
          PopupMenuButton(
            child: const ShadAvatar(
              'https://app.requestly.io/delay/2000/avatars.githubusercontent.com/u/124599?v=4',
              placeholder: Text('CN'),
            ),
            itemBuilder: (BuildContext context) => <PopupMenuEntry>[
              // Tutaj umieść elementy menu, które mają być wyświetlane, gdy użytkownik naciśnie Avatar
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.settings,
                ),
                onTap: () {
                  context.go('/settings');
                },
              ),
            ],
          ),
        ],
      ),
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}
