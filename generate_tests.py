import random
import math
from sympy import divisors
import pandas as pd


def generate_edge_and_special_cases(interval_end, curr_special_case_n, special_case_n, interval_sign):
    """ Generate edge cases and special cases for the interval """
    square_root = int(math.sqrt(interval_end))
    examples_in_interval = set()
    examples_in_interval.add((1, interval_end, interval_sign))
    examples_in_interval.add((square_root, square_root, interval_sign))

    # Generate special case pairs where n * n^2 is within the interval
    while curr_special_case_n * (curr_special_case_n ** 2) <= interval_end:
        A = curr_special_case_n
        B = curr_special_case_n ** 2
        examples_in_interval.add((A, B, interval_sign))
        curr_special_case_n += special_case_n

    return curr_special_case_n, examples_in_interval


def generate_divisor_pairs(interval_end, num_examples_per_interval, examples_in_interval, interval_sign):
    """ Generate divisor pairs for the interval based on the number of examples required """
    square_root = int(math.sqrt(interval_end))
    pos_divisors = [divisor for divisor in divisors(interval_end) if divisor not in [1, interval_end, square_root]]
    curr_num_examples = len(examples_in_interval)

    # If there are more divisors than needed, randomly sample the required number
    if len(pos_divisors) > num_examples_per_interval - curr_num_examples:
        nums = random.sample(pos_divisors, num_examples_per_interval - curr_num_examples)
        for A in nums:
            B = interval_end // A
            examples_in_interval.add((A, B, interval_sign))
    else:
        # Otherwise, add all divisors
        for A in pos_divisors:
            B = interval_end // A
            examples_in_interval.add((A, B, interval_sign))

    return examples_in_interval


def generate_by_intervals(intervals, num_examples_per_interval, special_case_n):
    """ Generate test examples for each interval """
    examples = []
    curr_special_case_n = special_case_n

    for i in range(len(intervals) - 1):
        interval_start, interval_end = intervals[i], intervals[i + 1] - 1
        interval_sign = f'{interval_start}-{interval_end}'

        # Generate edge and special cases for the current interval
        curr_special_case_n, examples_in_interval = generate_edge_and_special_cases(
            interval_end, curr_special_case_n, special_case_n, interval_sign
        )

        curr_num_examples = len(examples_in_interval)
        if curr_num_examples >= num_examples_per_interval:
            if curr_num_examples > num_examples_per_interval:
                examples_in_interval = random.sample(sorted(examples_in_interval), num_examples_per_interval)
            examples.extend(list(examples_in_interval))
            continue

        examples_in_interval = generate_divisor_pairs(
            interval_end, num_examples_per_interval, examples_in_interval, interval_sign
        )

        examples.extend(list(examples_in_interval))

    return examples


def generate_tests(args):
    """ Main function to generate test examples """
    intervals = list(range(args['S_min'], args['S_max'] + 1, args['S_max'] // args['num_intervals']))
    intervals.append(args['S_max'] + 1)

    examples = generate_by_intervals(intervals, args['num_examples_per_interval'], args['special_case_n'])
    examples = [(1, 1, '0-1')] + examples
    df_tests = pd.DataFrame(columns=['A', 'B', 'S_interval'])
    for (A, B, S) in examples:
        df_tests.loc[df_tests.shape[0]] = [A, B, S]
    df_tests.to_csv(args['output_file'], index=False)

    n = len(examples)
    if args['verbose']:
        for i, (A, B, S) in enumerate(examples[:5]):
            print(f"Example {i + 1}/{n}: A = {A}, B = {B}, S = {S}")
        print('...')
        for i, (A, B, S) in enumerate(examples[-5:]):
            print(f"Example {n - (5 - i) + 1}/{n}: A = {A}, B = {B}, S = {S}")


if __name__ == '__main__':
    import json

    args = json.load(open("./configs/test_gen_config.json"))
    generate_tests(args)
