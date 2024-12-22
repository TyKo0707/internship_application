import random
import math
from sympy import divisors
import pandas as pd
from tqdm import tqdm


def generate_edge_and_special_cases(interval_end, curr_special_case_k, special_case_k, interval_sign):
    """ Generate edge cases and special cases for the interval """
    square_root = int(math.sqrt(interval_end))
    examples_in_interval = set()
    examples_in_interval.add((1, interval_end, interval_sign))
    examples_in_interval.add((square_root, square_root, interval_sign))

    # Generate special case pairs where k * k^2 is within the interval
    while curr_special_case_k * (curr_special_case_k ** 2) <= interval_end:
        A = curr_special_case_k
        B = curr_special_case_k ** 2
        examples_in_interval.add((A, B, interval_sign))
        curr_special_case_k += special_case_k

    return curr_special_case_k, examples_in_interval


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


def generate_tests(intervals, num_examples_per_interval, special_case_k):
    """ Generate test examples for each interval """
    examples = []
    curr_special_case_k = special_case_k

    for i in tqdm(range(len(intervals) - 1)):
        interval_start, interval_end = intervals[i], intervals[i + 1] - 1
        interval_sign = f'{interval_start}-{interval_end}'

        # Generate edge and special cases for the current interval
        curr_special_case_k, examples_in_interval = generate_edge_and_special_cases(
            interval_end, curr_special_case_k, special_case_k, interval_sign
        )

        curr_num_examples = len(examples_in_interval)
        if curr_num_examples >= num_examples_per_interval:
            examples.extend(list(examples_in_interval))
            continue

        examples_in_interval = generate_divisor_pairs(
            interval_end, num_examples_per_interval, examples_in_interval, interval_sign
        )

        examples.extend(list(examples_in_interval))

    return examples


if __name__ == '__main__':
    args = {
        'num_examples_per_interval': 10,
        'num_intervals': 50,
        'S_min': 1,
        'S_max': 1000000,
        'special_case_k': 5,
        'output_file': "./data/tests/tests.csv",
        'verbose': 0,
    }

    intervals = list(range(args['S_min'], args['S_max'] + 1, args['S_max'] // args['num_intervals']))
    intervals[-1] = args['S_max'] + 1

    examples = generate_tests(intervals, args['num_examples_per_interval'], args['special_case_k'])
    df_tests = pd.DataFrame(columns=['A', 'B', 'S_interval'])
    for (A, B, S) in examples:
        df_tests.loc[df_tests.shape[0]] = [A, B, S]

    df_tests.to_csv(args['output_file'], index=False)
    if args['verbose']:
        for i, (A, B, S) in enumerate(examples):
            print(f"Example {i + 1}: A = {A}, B = {B}, S = {S}")
