export function CompareNotIn<S, T>(source: S[], target: T[], func: (s: S, t: T) => boolean) {
  const result: S[] = [];
  for (let index = 0; index < source.length; index++) {
    const s = source[index];

    let isHave = false;
    for (let tIndex = 0; tIndex < target.length; tIndex++) {
      const t = target[tIndex];

      if (func(s, t)) {
        isHave = true;
        break;
      }
    }
    if (!isHave) {
      result.push(s);
    }
  }
  return result;
}
