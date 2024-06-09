// ignore_for_file: public_member_api_docs, sort_constructors_first
// ignore: implementation_imports
import 'package:flutter/material.dart';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class Logo extends HookConsumerWidget {
  final double size;

  const Logo({
    super.key,
    required this.size,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Column(
      children: [
        ShadImage.square(
          'lib\\assets\\logo_white.png',
          size: size,
        ),
        const Text(
          style: TextStyle(color: Colors.white),
          'ONYX',
          textAlign: TextAlign.center,
        )
      ],
    );
  }
}
