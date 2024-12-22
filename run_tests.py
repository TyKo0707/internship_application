import pandas as pd
import subprocess
import sys
from tqdm import tqdm


def parse_output(result_stdout, method, A, B):
    """Parses the output from the C# executable."""
    if method == "spiral":
        max_side = max(A, B)
        moves_count = max_side ** 2
        elapsed_time = 0
    else:
        output_lines = result_stdout.strip().split("\n")[-2:]
        moves_count = int(output_lines[0].strip().split(':')[-1])
        elapsed_time = float(output_lines[1].strip().split(':')[-1].replace(',', '.'))
    return moves_count, elapsed_time


def process_row(row, verbose, method, generate_moves):
    """Process a single row from the DataFrame."""
    A, B = row["A"], row["B"]
    S_interval = row["S_interval"]

    command = f"dotnet run --project ./Snake/Snake/ {A} {B} {verbose} {method} {generate_moves}"

    try:
        result = subprocess.run(
            command, shell=True, capture_output=True, text=True, check=True
        )
        moves_count, elapsed_time = parse_output(result.stdout, method, A, B)
        new_row = {"A": A, "B": B, "S_interval": S_interval, "TurnsCount": moves_count, "ElapsedTime": elapsed_time}
        return new_row
    except subprocess.CalledProcessError as e:
        print(f"Error while calling C# solution: {e}", file=sys.stderr)
        return None


def run(args):
    """Main function to run the process for all rows in the input file."""
    input_file = args['test_path']
    output_file = args["output_path"]

    df = pd.read_csv(input_file)
    output_data = []

    for i, row in tqdm(df.iterrows(), total=df.shape[0]):
        result_row = process_row(row, args['verbose'], args['method'], args['generate_moves'])
        if result_row:
            output_data.append(result_row)

    output_df = pd.DataFrame(output_data)
    output_df.to_csv(output_file, index=False)


if __name__ == '__main__':
    args = {
        'verbose': 0,
        'generate_moves': 0,
        'method': 'lcm_zigzag',
        'test_path': './data/tests/tests.csv',
    }
    args['output_path'] = f'./data/results/{args["method"]}_results.csv'
    run(args)
