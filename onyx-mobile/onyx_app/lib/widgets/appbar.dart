import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';

class DefaultltAppBar extends HookConsumerWidget
    implements PreferredSizeWidget {
  final String title;
  const DefaultltAppBar({
    Key? key,
    required this.title,
  }) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return AppBar(
      title: Text(title),
      leading: IconButton(
        icon: const Icon(Icons.menu),
        tooltip: 'Menu Icon',
        onPressed: () {},
      ),
      actions: [
        IconButton(
          icon: const Icon(Icons.settings),
          tooltip: 'Setting Icon',
          onPressed: () {},
        )
      ],
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}
