import argparse
import json
import run_tests
import warnings

warnings.filterwarnings('ignore')

config = json.load(open("./configs/main_config.json"))
parser = argparse.ArgumentParser()

# Evaluation arguments
parser.add_argument("--verbose", default=config['verbose'], type=int,
                    help="Level of verbosity for output.")
parser.add_argument("--generate_moves", default=config['generate_moves'], type=int,
                    help="Flag to indicate whether to generate new moves")
parser.add_argument("--method", default=config['choice'], type=str,
                    help="Algorithm or strategy to use for movement simulation")
parser.add_argument("--test_path", default='./data/tests/tests.csv', type=str,
                    help="Relative path to the test file from current file")
parser.add_argument("--plot", default=True, type=bool,
                    help="Flag to indicate whether to plot the results of evaluation")

# Test generation arguments
parser.add_argument("--generate_test_set", default=True, type=bool,
                    help="Flag to indicate whether to generate new test dataset")
parser.add_argument("--test_set_config_path", default='./configs/test_gen_config.json', type=str,
                    help="Path to test generation config file")


def evaluate(args):
    # Step 1: Generate new test set if stated
    if args['generate_test_set']:
        import generate_tests
        print('Generating new test set...')
        test_gen_args = json.load(open(args['test_set_config_path']))
        generate_tests.generate_tests(test_gen_args)
        args['test_path'] = test_gen_args['output_file']
        print(f'Test set generated successfully and saved to file at {test_gen_args["output_file"]}\n')

    # Step 2: Run the tests
    print('Running tests...')
    args['output_path'] = f'./data/results/{args["method"]}_results1.csv'
    run_tests.run(args)
    print(f'Tests run successfully, results saved to file at {args["output_path"]}\n')

    # Step 3: Plot the results
    if args['plot']:
        import build_plot
        args['input_path'] = args['output_path']
        args['output_path'] = f'./img/{args["method"]}_results1.png'
        print('Plotting results...')
        build_plot.plot_results(args)
        print(f'Plot saved to file at {args["output_path"]}')


if __name__ == '__main__':
    args = parser.parse_args([] if "__file__" not in globals() else None)
    evaluate(vars(args))
